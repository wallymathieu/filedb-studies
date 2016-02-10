require 'albacore'
require 'fileutils'
require 'nuget_helper'

include FileUtils

task :default => [:all]

desc "Rebuild solution"
build :build do |msb, args|
  msb.prop :configuration, :Debug
  msb.target = [:Rebuild]
  msb.sln = "filedb-studies.sln"
end

desc "Install missing NuGet packages."
nugets_restore :restore do |p|
    p.out = 'packages'
    p.nuget_gem_exe
end

desc "test using console"
test_runner :test => [:build] do |runner|
  runner.exe = NugetHelper::nunit_path
  d = File.dirname(__FILE__)
  files = Dir.glob(File.join(d,"**", "bin", "Debug","*Tests.dll"))
  runner.files = files 
end

